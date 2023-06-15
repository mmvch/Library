import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookCreateComponent } from './book-create/book-create.component';
import { BookEditComponent } from './book-edit/book-edit.component';
import { BookListComponent } from './book-list/book-list.component';
import { BookComponent } from './book/book.component';

const routes: Routes = [
    {
        path: '',
        component: BookListComponent
    },
    {
        path: 'book/create',
        component: BookCreateComponent
    },
    {
        path: 'book/:id',
        component: BookComponent
    },
    {
        path: 'book/:id/edit',
        component: BookEditComponent
    },
    {
        path: '**',
        redirectTo: ''
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BookRoutingModule { }
