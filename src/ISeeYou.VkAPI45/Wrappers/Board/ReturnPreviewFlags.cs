#region Using

using System;

#endregion

namespace VkAPIAsync.Wrappers.Board
{
    public class ReturnPreviewFlags
    {
        private bool _first;
        private bool _last;
        public int Value { get; private set; }

        public ReturnPreviewFlags FirstComment()
        {
            if (_first)
                throw new Exception("Метод уже вызывался");
            Value += 1;
            _first = true;
            return this;
        }

        public ReturnPreviewFlags LastComment()
        {
            if (_last)
                throw new Exception("Метод уже вызывался");
            Value += 2;
            _last = true;
            return this;
        }
    }
}