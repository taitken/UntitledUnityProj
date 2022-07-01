namespace System
{
    public class BaseModel
    {
        private static long idIncrementer = 1;
        public long ID { get; set; }

        public BaseModel(){
            this.ID = idIncrementer;
            idIncrementer++;
        }
    }
}