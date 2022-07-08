export class User
{
    Email      :string ;
    Username   :string ;
    Name       :string ;
    Surname    :string ;
    Birthdate  :string ;
    Address    :string ;
    Type       :string ;
    ImageRaw;

    constructor(Email:string, Username:string, Name:string, 
                Surname:string, Birthdate:string, Address:string, Type:string, ImageRaw)
    {
      this.Email        =Email;    
      this.Username     =Username; 
      this.Name         =Name;     
      this.Surname      =Surname; 
      this.Birthdate    =Birthdate;
      this.Address      =Address; 
      this.Type         =Type;   
      this.ImageRaw     =ImageRaw;
    };

    public get Img(): File {
        return this.ImageRaw;
      }
      public set Img(value: File) {
        this.ImageRaw = value;
      }
}