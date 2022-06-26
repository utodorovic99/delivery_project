import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogginComponent } from './components/loggin/loggin.component';

const routes: Routes = [
  {path:'home', component:LogginComponent, children:
  [
    {path:'login',    component: LogginComponent},
    {path:'register', component: LogginComponent},
  ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
export const routingComponents = [LogginComponent]
