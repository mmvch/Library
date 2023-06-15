import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Book } from 'src/app/models/book';
import { BookService } from 'src/app/services/book.service';

@Component({
    selector: 'app-book-create',
    templateUrl: './book-create.component.html',
    styleUrls: ['./book-create.component.scss']
})
export class BookCreateComponent implements OnInit {
    @Input() book?: Book;
    imageFile: any;
    textFile: any;

    constructor(private router: Router, private toastr: ToastrService, private bookService: BookService) {
    }

    ngOnInit(): void {
        this.book = new Book();
        window.scrollTo({ top: 0 });
    }

    createBook(book: Book) {
        const formData: FormData = new FormData();

        formData.append("name", book.name ?? "");
        formData.append("description", book.description ?? "");

        if (this.imageFile) {
            formData.append("image", this.imageFile);
        }

        if (this.textFile) {
            formData.append("bookText", this.textFile);
        }

        this.bookService.create(formData).subscribe({
            next: () => {
                this.return();
            },
            error: (error: any) => {
                if (error.error.errors) {
                    this.toastr.error(error.error.errors.Name[0]);
                }
                else {
                    this.toastr.error(error.error.message);
                }
            },
            complete: () => {
                this.toastr.success("Updating successful.");
            }
        });
    }

    return() {
        this.router.navigate(['']);
    }

    selectImageFile(event: any) {
        this.imageFile = event?.target?.files[0];
    }

    selectTextFile(event: any) {
        this.textFile = event?.target?.files[0];
    }
}
