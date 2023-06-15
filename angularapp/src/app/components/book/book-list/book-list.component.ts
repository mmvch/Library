import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/models/book';
import { PartialData } from 'src/app/models/partial-data';
import { BookService } from 'src/app/services/book.service';

@Component({
    selector: 'app-book-list',
    templateUrl: './book-list.component.html',
    styleUrls: ['./book-list.component.scss']
})
export class BookListComponent implements OnInit {
    books?: Book[];
    name?: string;
    totalAmount = 0;
    currentPage = 1;
    pageSize = 10;

    get pageCount(): number {
        return Math.ceil(this.totalAmount / this.pageSize);
    }

    constructor(private bookService: BookService) { }

    ngOnInit(): void {
        const params = new HttpParams()
            .set("name", this.name ?? "")
            .set("currentPage", this.currentPage)
            .set("pageSize", this.pageSize);

        this.bookService.list(params).subscribe((result: PartialData<Book>) => {
            this.books = result.data;
            this.totalAmount = result.totalAmount ?? 0;
        });
    }

    clear() {
        this.name = "";
        this.ngOnInit();
    }

    search() {
        this.currentPage = 1;
        this.ngOnInit();
    }

    getPage(pageNumber: number) {
        this.currentPage = pageNumber;
        this.ngOnInit();
    }

    paginate(): number[] {
        let pageCount = this.pageCount;
        let pages = new Set<number>();
        let counter = 1;

        pages.add(1);
        pages.add(this.currentPage);
        pages.add(pageCount);

        while (pages.size < 5 && pages.size < pageCount) {
            let targetPage = this.currentPage + counter;

            if (targetPage >= 1 && targetPage <= pageCount) {
                pages.add(targetPage);
            }

            targetPage = this.currentPage - counter;

            if (targetPage >= 1 && targetPage <= pageCount) {
                pages.add(targetPage);
            }

            counter++;
        }

        pages.delete(1);

        let pagesArray = Array.from(pages).sort((a, b) => a - b);
        let result = [1];

        pagesArray.forEach(element => {
            if (element - 1 > result[result.length - 1]) {
                result.push(0);
            }

            result.push(element);
        });

        return result;
    }
}
