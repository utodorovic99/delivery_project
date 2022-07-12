export class UserRegisterRequest
{
    Email      :string ;
    Password   :string ;
    Re_password:string ;
    Username   :string ;
    Name       :string ;
    Surname    :string ;
    Birthdate  :string ;
    Address    :string ;
    Type       :string ;
    ImageRaw : File;

    

    constructor(Email:string, Password:string, Re_password:string, Username:string, Name:string, 
                Surname:string, Birthdate:string, Address:string, Type:string, ImageRaw:File)
    {
      this.Email        =Email;    
      this.Password     =Password; 
      this.Re_password  =Re_password;
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