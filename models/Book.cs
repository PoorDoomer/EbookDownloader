using System;

namespace Book{
public class BookModel
    {
        public string file_path { get; set; }
        public string name { get; set; }
        public bool bookmarked { get; set; } 
        public string page_reached {get; set;}
        public DateTime last_time_read  {get; set;}  
               
        public void State()   
        {   
            Console.WriteLine($"You read {name} {last_time_read} and the state of its bookmarked is {bookmarked} ");
        }  
        public void Bookmark(){   
            this.bookmarked = true;    
        }   
           
        public void JustRead(){   
            this.last_time_read = DateTime.Now;   
        }

        public BookModel(string file_path, string name,bool bookmarked,string  page_reached,DateTime last_time_read){
                this.file_path = file_path;
                this.name = name;
                this.bookmarked = bookmarked;
                this.page_reached = page_reached;
                this.last_time_read = last_time_read;
        }
   
   
    };  
 


}
