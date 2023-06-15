import { Component, Input, OnInit } from '@angular/core';
import { Text } from 'src/app/models/text';
import { TextService } from 'src/app/services/text.service';

@Component({
  selector: 'app-book-text',
  templateUrl: './book-text.component.html',
  styleUrls: ['./book-text.component.scss']
})
export class BookTextComponent implements OnInit {
  @Input() id?: string;
  text?: Text;
  currentPage = 1;
  itemsPerPage = 1;

  constructor(private textService: TextService) { }

  ngOnInit(): void {
    if (this.id) {
      this.textService.get(this.id).subscribe((result: Text) => (this.text = result));
    }
  }

  handlePageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    window.scrollTo({ top: 480, behavior: 'smooth' });
  }
}
