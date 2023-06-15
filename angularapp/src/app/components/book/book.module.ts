import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BookRoutingModule } from './book-routing.module';
import { FormsModule } from '@angular/forms';
import { BookListComponent } from './book-list/book-list.component';
import { BookEditComponent } from './book-edit/book-edit.component';
import { BookCardComponent } from './book-card/book-card.component';
import { BookCreateComponent } from './book-create/book-create.component';
import { BookTextComponent } from './book-text/book-text.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { BookComponent } from './book/book.component';

@NgModule({
    declarations: [
        BookListComponent,
        BookEditComponent,
        BookCreateComponent,
        BookCardComponent,
        BookTextComponent,
        BookComponent
    ],
    imports: [
        CommonModule,
        BookRoutingModule,
        NgxPaginationModule,
        FormsModule
    ],
})
export class BookModule { }
