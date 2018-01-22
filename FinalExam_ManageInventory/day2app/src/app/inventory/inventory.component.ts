import { Component, OnInit } from '@angular/core';
import { Inventory } from '../domain/inventory';
import { InventoryClass } from '../domain/inventoryclass';

import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { MenuItem } from 'primeng/primeng';
import { ConfirmationService } from 'primeng/primeng';
import { GlobalService } from "../services/globalservice";
import { AuthService } from "../services/auth.service";

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
  providers: [ConfirmationService, GlobalService]
})
export class InventoryComponent implements OnInit {

  inventory: Inventory = new InventoryClass();
  inventoryList: Inventory[];
  selectedInventory: Inventory;
  cloneInventory: Inventory;
  isNewVenue: boolean;

  items: MenuItem[];
  home: MenuItem;

  constructor(private globalService: GlobalService, private fb: FormBuilder, private conf: ConfirmationService
    , public auth: AuthService) { }

  ngOnInit() {
    this.globalService.getSomething("inventories")
      .then(inventory => this.inventoryList = inventory);

    this.items = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Employee', routerLink: ['/employees'] },
      { label: 'Contact', routerLink: ['/contacts'] },
      { label: 'Venue', routerLink: ['/venues'] },
      { label: 'Appointment', routerLink: ['/appointments'] }
    ];

    this.home = { icon: 'fa fa-home' };
  }

}
