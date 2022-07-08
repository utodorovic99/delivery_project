export class UserUpdateRequest
{
    Name        :string ;
    Surname     :string ;
    Birthdate   :string ;
    Address     :string ;
    Password    :string ;
    NewPassword :string ;
    Email       :string;
    ImageRaw;

    constructor(Email:string, Name:string, Surname:string, Birthdate:string, Address:string,
                Password:string, NewPassword:string,ImageRaw)
    {
      this.Name         = Name;     
      this.Surname      = Surname; 
      this.Birthdate    = Birthdate;
      this.Address      = Address; 
      this.Password     = Password;
      this.NewPassword  = NewPassword;
      this.ImageRaw     = ImageRaw;
      this.Email = Email;
    };

    public get Img(): File {
        return this.ImageRaw;
      }
      public set Img(value: File) {
        this.ImageRaw = value;
      }
}