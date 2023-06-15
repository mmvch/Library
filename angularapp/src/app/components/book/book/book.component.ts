import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { Book } from 'src/app/models/book';
import { BookService } from 'src/app/services/book.service';
import { Jwt, Roles } from 'src/environments/const';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-book',
    templateUrl: './book.component.html',
    styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {
    book?: Book;
    public environment = environment;

    constructor(private route: ActivatedRoute, private router: Router, private toastr: ToastrService, private bookService: BookService) { }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.bookService.get(id).subscribe({
                next: (response) => {
                    this.book = response;
                },
                error: (error: any) => {
                    this.toastr.error(error.error.message);
                }
            });
        }

        window.scrollTo({ top: 0 });
    }

    deleteBook(id: string) {
        this.bookService.delete(id).subscribe({
            next: () => {
                this.return();
            },
            error: (error: any) => {
                this.toastr.error(error.error.message);
            },
            complete: () => {
                this.toastr.success("Deleting successful.");
            }
        });
    }

    isAuthorOrAdmin(authorId?: string): boolean {
        if (authorId) {
            const token = localStorage.getItem('authToken');

            if (token) {
                const decodeToken: any = jwtDecode(token);

                if (decodeToken[Jwt.role].includes(Roles.admin) || decodeToken[Jwt.nameIdentifier] === authorId) {
                    return true;
                }
            }
        }

        return false;
    }

    return() {
        this.router.navigate(['']);
    }
}
