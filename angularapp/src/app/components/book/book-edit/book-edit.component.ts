import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Book } from 'src/app/models/book';
import { BookService } from 'src/app/services/book.service';

@Component({
    selector: 'app-book-edit',
    templateUrl: './book-edit.component.html',
    styleUrls: ['./book-edit.component.scss']
})
export class BookEditComponent implements OnInit {
    @Input() book?: Book;
    imageFile: any;
    textFile: any;

    constructor(private route: ActivatedRoute, private router: Router, private toastr: ToastrService, private bookService: BookService) { }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.bookService.get(id).subscribe(response => (this.book = response));
        }
        window.scrollTo({ top: 0 });
    }

    updateBook(book: Book) {
        const formData: FormData = new FormData();

        formData.append("id", book.id ?? "");
        formData.append("name", book.name ?? "");
        formData.append("description", book.description ?? "");

        if (this.imageFile) {
            formData.append("image", this.imageFile);
        }

        if (this.textFile) {
            formData.append("bookText", this.textFile);
        }

        this.bookService.update(formData).subscribe({
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
        if (this.book?.id) {
            this.router.navigate(['book', this.book.id]);
        } else {
            this.router.navigate(['']);
        }
    }

    selectImageFile(event: any) {
        this.imageFile = event?.target?.files[0];
    }

    selectTextFile(event: any) {
        this.textFile = event?.target?.files[0];
    }
}
