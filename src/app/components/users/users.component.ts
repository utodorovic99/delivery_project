import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { UserService } from 'src/app/services/user.service';
import { DataSource } from '@angular/cdk/collections';
import { DomSanitizer } from '@angular/platform-browser';

export interface UserView{
  email      :string ;
  username   :string ;
  name       :string ;
  surname    :string ;
  birthdate  :string ;
  address    :string ;
  type       :string ;
  state      :string ;
  image;
}

const ELEMENT_DATA_USERS: UserView[] = [];

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit, AfterViewInit {
 @ViewChild(MatTable)table!:MatTable<UserView>;
 @ViewChild(MatPaginator) paginator!: MatPaginator
 displayedColumnsUsers: string[] = ['image',     'email',     'username',  'name', 'surname', 
                                    'birthdate', 'address',   'type',       'state'];

 dataSourceUsers = new MatTableDataSource(ELEMENT_DATA_USERS);
 verifyWindowVisible=false;
 selectedUserUsername="";
 selectedUserRow="";
constructor(private http: HttpClient,private userService:UserService, private sanitizer: DomSanitizer) { }

  ngOnInit(): void 
  {
    this.userService.getAllUsers(this.http).subscribe(
      {
        next: (data)=>
        {
          ELEMENT_DATA_USERS.splice(0);
          var url;
          var urlPass;
          for(let user in data as unknown as UserView[])
          {     
            if((data[user]['imageRaw']!=='AA==' || data[user]['imageRaw']!=='')  && data[user]['imageRaw'].length>0)
            {
              url = 'data:image/png;base64,' + data[user]['imageRaw'];
              urlPass= this.sanitizer.bypassSecurityTrustUrl(url);
            }
            else urlPass = 'assets\\Images\\select_image.png'
            
            ELEMENT_DATA_USERS.push({ image:urlPass,                       email:data[user]['email'], 
                                      username:data[user]['username'],     name:data[user]['name'],        surname:data[user]['surname'], birthdate: data[user]['birthdate'],  
                                      address:data[user]['address'],       type:data[user]['type'],        state: this.GetUserSyatysByCode(data[user]['state'])});
          }
          this.dataSourceUsers = new MatTableDataSource(ELEMENT_DATA_USERS);
          this.table.renderRows(); 
        },

        error: (error)=>
        {
          console.log(error);
          //
        }
      }); 
  }

  ngAfterViewInit():void
  {
    setTimeout(() =>{
      this.dataSourceUsers.paginator = this.paginator;
    },60);   
  }

  private GetUserSyatysByCode(code:number):string
  {
    //{ Unconfirmed = 0, Confirmed = 1, Rejected = 2, Pending=3 }
    switch(code)
    {
      case 0: {return "unconfirmed";}
      case 1: {return "confirmed";  }
      case 2: {return "rejected";   }
      case 3: {return "pending";    }
      default:{return "unknown";    } 
    }
  }

  tableUsersRowClick(rowUser)
  {
    if(rowUser['type'] == 'deliveryman') 
    {
      this.verifyWindowVisible = true;
      this.selectedUserUsername = rowUser['username'];
      this.selectedUserRow = rowUser
    }
    else
    { 
      this.verifyWindowVisible = false;
      this.selectedUserUsername ="";
    }
  }

  tryUserReject()
  {
    this.userService.userReject(this.selectedUserRow['username'], this.http).subscribe(
      {
        next: (data)=>
        {
          this.updateUserState(this.selectedUserRow['username'],'rejected');
          this.table.renderRows(); 
        },

        error: (error)=>
        {
          //
        }
      }); 
  }

  tryUserVerification()
  {
    this.userService.userVerification(this.selectedUserRow['username'], this.http).subscribe(
      {
        next: (data)=>
        {
          this.updateUserState(this.selectedUserRow['username'],'pending');
        },

        error: (error)=>
        {
          //
        }
      })
  }

  tryUserConfirm()
  {
    this.userService.userVerify(this.selectedUserRow['username'], this.http).subscribe(
      {
        next: (data)=>
        {
          this.updateUserState(this.selectedUserRow['username'],'confirmed');
        },

        error: (error)=>
        {
          //
        }
      })
  }

  private updateUserState(username:string, newState:string)
  {
    for(let user in ELEMENT_DATA_USERS)
    {
      if(ELEMENT_DATA_USERS[user]['username']==username)
      {
        ELEMENT_DATA_USERS[user]['state'] = newState;
        break;
      }
    }
    this.table.renderRows(); 
  }
}
