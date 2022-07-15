export class UserUpdateRequest
{
  Email:string;
  Password :string;
  Username:string;
  Name:string;
  Surname:string;
  Birthdate:string;
  Address:string;
  Type:string;
  NewPassword:string;

    constructor(Email:string,Password :string,Username:string,Name:string,Surname:string,Birthdate:
      string,Address:string,Type:string,NewPassword:string)

    {
      this.Email        = Email;
      this.Password     = Password;
      this.Username     = Username;
      this.Name         = Name;     
      this.Surname      = Surname; 
      this.Birthdate    = Birthdate;
      this.Address      = Address; 
      this.Type         = Type;
      this.NewPassword  = NewPassword;
      
    };
}