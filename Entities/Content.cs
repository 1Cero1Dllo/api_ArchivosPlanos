namespace API.Entities
{
    public class Content
    {

        public Content()
        {
            this.flag = true;
            this.message = "";
        }
        public Content(bool flag, string ms)
        {
            this.flag = flag;
            this.message = ms;
        }

        public bool flag {get;  set;}
        public string message {get; set;}

    }
}