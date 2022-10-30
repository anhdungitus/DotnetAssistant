import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {HomeComponent} from "./home/home.component";
import {AuthGuard} from "./auth.guard";
import {PlanComponent} from "./plan/plan.component";

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'account',
        loadChildren: () => import('./account/account.module').then(m => m.AccountModule)
      },
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
      },
      {
        path: '',
        component: HomeComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'plan',
        component: PlanComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }


