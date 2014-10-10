namespace VkAPIAsync.Wrappers.Audios
{
    /// <summary>
    /// Жанр аудиозаписи
    /// </summary>
    public class AudioGenre
    {
        /// <summary>
        /// Перечисление жанров аудиозаписей
        /// </summary>
        public enum AudioGenreEnum
        {
            Rock = 1,
            Pop = 2,
            RapAndHipHop = 3,
            EasyListening = 4,
            DanceAndHouse = 5,
            Instrumental = 6,
            Metal = 7,
            Alternative = 21,
            Dubstep = 8,
            JazzAndBlues = 9,
            DrumNBass = 10,
            Trance = 11,
            Chanson = 12,
            Ethnic = 13,
            AcousticAndVocal = 14,
            Regae = 15,
            Classical = 16,
            IndiePop = 17,
            Speech = 19,
            Disco = 22,
            Other = 18
        }

        public AudioGenre(AudioGenreEnum e)
        {
            Value = (int)e;
        }

        public int Value { get; set; }

        public override string ToString()
        {
            return Value > 0 ? ((AudioGenreEnum)Value).ToString("g") : base.ToString();
        }
    }
}
