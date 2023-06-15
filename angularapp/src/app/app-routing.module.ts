import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './components/user/auth/auth.component';

const routes: Routes = [
    {
        path: 'auth/login',
        component: AuthComponent
    },
    {
        path: 'auth/register',
        component: AuthComponent
    },
    {
        path: '',
        loadChildren: () => import('./components/book/book.module').then(m => m.BookModule)
    },
    {
        path: '**',
        redirectTo: ''
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
