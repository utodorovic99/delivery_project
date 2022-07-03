export class UserRegisterRequest
{
    email      ="";
    password   ="";
    re_password="";
    username   ="";
    name       ="";
    surname    ="";
    birthdate  ="";
    address    ="";
    type       ="";
    imageRaw;

    public get Img(): File {
        return this.imageRaw;
      }
      public set Img(value: File) {
        this.imageRaw = value;
      }
}