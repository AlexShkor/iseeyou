#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    public class LongPollServerEventArgs : EventArgs
    {
        /// <summary>
        ///     Полный текст JSON ответа
        /// </summary>
        public string EventSourceCode;

        /// <summary>
        ///     Последний TimeStamp
        /// </summary>
        public long LastEventId = 0;
    }

    /// <summary>
    ///     Класс, представляющий LongPoll подключение
    /// </summary>
    public class LongPollServerConnection : Object
    {
        private readonly object _locker = new object();

        private readonly List<string> _longPollMessages;

        private Task _connectionListenerThread;
        private string _lastLongPollMessage;
        private volatile bool _stopPending;
        private SynchronizationContext _sync;

        private int _waitTime = 25;

        /// <summary>
        ///     Создает LongPoll подключение
        /// </summary>
        public LongPollServerConnection()
        {
            _longPollMessages = new List<string>();
        }

        /// <summary>
        ///     Макс. время ожидания ответа. По-умолчанию = 25 секунд.
        /// </summary>
        public int WaitTime
        {
            get { return _waitTime; }
            set { _waitTime = value; }
        }

        private LongPollServerConnectionInfo ConnectionInfo { get; set; }

        /// <summary>
        ///     Вызывается когда информация от LongPoll-сервера получена
        /// </summary>
        public event EventHandler<LongPollServerEventArgs> ReceivedData;

        /// <summary>
        ///     Вызывается когда LongPoll сервер остановлен
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        ///     Вызывается когда LongPoll-сервер прислал какую-то информацию
        /// </summary>
        protected virtual void OnReceivedData(LongPollServerEventArgs e)
        {
            if (ReceivedData != null)
            {
                ReceivedData(this, e);
            }
        }

        /// <summary>
        ///     Останавливает LongPoll-клиент
        /// </summary>
        protected virtual void OnStopped()
        {
            if (Stopped != null)
            {
                Stopped(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Начинает обработку ответов сервера
        /// </summary>
        public void Start()
        {
            _sync = SynchronizationContext.Current;
            _stopPending = false;
            if (_connectionListenerThread != null)
            {
                if (_connectionListenerThread.Status == TaskStatus.Canceled)
                {
                    _connectionListenerThread = new Task(Run, _sync);
                    _connectionListenerThread.Start();
                }
                if (_connectionListenerThread.Status == TaskStatus.Created ||
                    _connectionListenerThread.Status == TaskStatus.WaitingToRun)
                {
                    _connectionListenerThread = new Task(Run, _sync);
                    _connectionListenerThread.Start();
                }
            }
            else
            {
                _connectionListenerThread = new Task(Run, _sync);
                _connectionListenerThread.Start();
            }
        }

        private async void Run(object state)
        {
            var context = state as SynchronizationContext;
            if (context == null) throw new ArgumentException("Параметр state должен быть типа SyncronizationContext");
            context.Send(GetConnectionInfo, null);
            context.Send(SetRequestTimeout, null);
            while (!_stopPending)
            {
                _lastLongPollMessage = await
                    ApiRequest.Send("http://" + ConnectionInfo.Server + "?act=a_check&key=" + ConnectionInfo.Key +
                                    "&ts=" + ConnectionInfo.Ts.Value.ToString(CultureInfo.InvariantCulture) +
                                    "&wait=" + WaitTime.ToString(CultureInfo.InvariantCulture));
                if (_lastLongPollMessage == "")
                {
                    continue;
                }
                // {"ts":727820493,"updates":[[8,-696076,0]]}
                if (new Regex("\\\"?failed\\\"?\\s*?\\:\\s*?\\d+").Match(_lastLongPollMessage).Success)
                {
                    context.Send(GetConnectionInfo, null);
                }
                else
                {
                    ConnectionInfo.Ts =
                        Convert.ToInt32(
                            new Regex("\\{[\\s]*?\\\"ts\\\"[\\s]*?\\:[\\s]*?(\\d+)[\\s]*?").Match(_lastLongPollMessage).
                                                                                            Groups[1].Value);
                    if (!string.IsNullOrEmpty(_lastLongPollMessage))
                    {
                        if (!new Regex("\\\"updates\\\"\\:\\[\\]").Match(_lastLongPollMessage).Success)
                        {
                            _longPollMessages.Add(_lastLongPollMessage);
                            var args = new LongPollServerEventArgs
                                {
                                    LastEventId = ConnectionInfo.Ts.Value,
                                    EventSourceCode = _lastLongPollMessage
                                };

                            context.Post(DoOnDataReceived, args);
                        }
                    }
                }
            }
        }

        private async void GetConnectionInfo(object data)
        {
            ConnectionInfo = await Messages.GetLongPollServer();
        }

        private void DoOnDataReceived(object data)
        {
            OnReceivedData((LongPollServerEventArgs) data);
        }

        private void SetRequestTimeout(object data)
        {
            ApiRequest.Timeout = WaitTime*1000;
        }

        /// <summary>
        ///     Заканчивает обработку ответов сервера
        /// </summary>
        public void Stop()
        {
            if (Stopped != null)
            {
                Stopped(this, EventArgs.Empty);
            }
            lock (_locker)
            {
                _stopPending = true;
            }
        }
    }
}