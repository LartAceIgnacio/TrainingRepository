import { Component, OnInit } from '@angular/core';
import { GlobalService } from '../services/globalservice';

@Component({
  selector: 'app-inventories',
  templateUrl: './inventories.component.html',
  styleUrls: ['./inventories.component.css'],
  providers: [GlobalService]
})
export class InventoriesComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
