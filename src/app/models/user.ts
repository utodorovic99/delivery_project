export class User
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
    img;

    public get Img(): File {
        return this.img;
      }
      public set Img(value: File) {
        this.img = value;
      }
}