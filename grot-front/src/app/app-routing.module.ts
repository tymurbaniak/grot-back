import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./grot/grot.module').then(m => m.GrotModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'grot',
    loadChildren: () => import('./grot/grot.module').then(m => m.GrotModule),
  },
  { 
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) 
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
