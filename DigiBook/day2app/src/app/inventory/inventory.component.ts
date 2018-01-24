import { Component, OnInit, ViewChild } from '@angular/core';
import { Inventory } from '../domain/inventory';
import { InventoryClass } from '../domain/inventoryclass';

import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Message, SelectItem } from 'primeng/components/common/api';
import { MenuItem, MenuModule } from 'primeng/primeng';
import { ConfirmationService } from 'primeng/primeng';
import { GlobalService } from "../services/globalservice";
import { AuthService } from "../services/auth.service";
import { Pagination } from "../domain/pagination";
import { DataTable } from "primeng/components/datatable/datatable";

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
  isNewInventory: boolean;
  prodCode: string;

  msgs: Message[] = [];

  userform: FormGroup;

  items: MenuItem[];

  home: MenuItem;

  description: string;

  display: boolean = false;

  edit: boolean = false;

  LL: SelectItem[];
  BB: SelectItem[];
  RR: SelectItem[];

  entity: string = "Inventories";
  searchQuery: string = "";
  totalRecord: number;

  constructor(private globalService: GlobalService, private fb: FormBuilder, private conf: ConfirmationService
    , public auth: AuthService) { }

  ngOnInit() {

    this.userform = this.fb.group({
      'productCode': new FormControl('', Validators.required),
      'productName': new FormControl('', Validators.required),
      'productDescription': new FormControl('', Validators.required),
      'qonHand': new FormControl('', Validators.required),
      'qonReserved': new FormControl('', Validators.required),
      'qonOrdered': new FormControl('', Validators.required),
      'binLL': new FormControl('', Validators.required),
      'binBB': new FormControl('', Validators.required),
      'binRR': new FormControl('', Validators.required)
    });

    this.LL = [
      {label: '01', value: '01'},
      {label: '02', value: '02'},
      {label: '03', value: '03'},
      {label: '04', value: '04'},
      {label: '05', value: '05'}
    ];

    this.BB = [
      {label: 'B1', value: 'B1'},
      {label: 'B2', value: 'B2'},
      {label: 'B3', value: 'B3'},
      {label: 'B4', value: 'B4'},
      {label: 'B5', value: 'B5'}
    ];

    this.RR = [
      {label: 'A', value: 'A'},
      {label: 'B', value: 'B'},
      {label: 'C', value: 'C'},
      {label: 'D', value: 'D'},
      {label: 'E', value: 'E'},
      {label: 'F', value: 'F'},
      {label: 'G', value: 'G'},
      {label: 'H', value: 'H'},
      {label: 'I', value: 'I'},
      {label: 'J', value: 'J'},
      {label: 'K', value: 'K'},
      {label: 'L', value: 'L'},
      {label: 'M', value: 'M'},
      {label: 'N', value: 'N'},
      {label: 'O', value: 'O'},
      {label: 'P', value: 'P'},
      {label: 'Q', value: 'Q'},
      {label: 'R', value: 'R'},
      {label: 'S', value: 'S'},
      {label: 'T', value: 'T'},
      {label: 'U', value: 'U'},
      {label: 'V', value: 'V'},
      {label: 'W', value: 'W'},
      {label: 'X', value: 'X'},
      {label: 'Y', value: 'Y'},
      {label: 'Z', value: 'Z'}
    ];

    this.items = [
      { label: 'Dashboard', routerLink: ['/dashboard'] },
      { label: 'Inventory', routerLink: ['/inventory'] }
    ];
    this.home = { icon: 'fa fa-home' };

  }

  newInventory() {
    this.display = true;
    this.selectedInventory = new InventoryClass();
    this.isNewInventory = true;
  }

  saveInventory() {
    let tmpInventoryList = [...this.inventoryList];
    this.selectedInventory.bin = this.selectedInventory.ll + this.selectedInventory.bb + this.selectedInventory.rr;
    this.selectedInventory.isActive = true;
    if (this.isNewInventory) {
      this.selectedInventory.dateModified = null;
      this.globalService.addSomething("Inventories", this.selectedInventory).then(emp => {
        tmpInventoryList.push(emp);
        this.inventoryList = tmpInventoryList;
        this.selectedInventory = null;
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Product Added!' });
      });
      this.display = false;
      this.edit = false;
    }

    else {
      this.globalService.updateSomething<Inventory>('Inventories', this.selectedInventory.productId, this.selectedInventory)
        .then(inventory => {
          tmpInventoryList[this.inventoryList.indexOf(this.cloneInventory)] = this.selectedInventory;
          this.inventoryList = tmpInventoryList;
          this.selectedInventory = null;
          this.msgs = [];
          this.msgs.push({ severity: 'success', summary: 'Product Saved!' });
          this.display = false;
          this.edit = false;
        });
    }
    this.userform.markAsPristine();
    this.isNewInventory = false;
  }

  saveAndNewInventory() {
    let tmpInventoryList = [...this.inventoryList];
    this.selectedInventory.bin = this.selectedInventory.ll + this.selectedInventory.bb + this.selectedInventory.rr;
    this.selectedInventory.isActive = true;
    this.globalService.addSomething("Inventories", this.selectedInventory).then(emp => {
      tmpInventoryList.push(emp);
      this.inventoryList = tmpInventoryList;
      this.selectedInventory = new InventoryClass();
    });
    this.userform.markAsPristine();
    this.msgs = [];
    this.msgs.push({ severity: 'success', summary: 'Product Added!' });
  }

  editInventory(inventory: Inventory) {
    this.edit = true;
    this.selectedInventory = inventory;
    this.isNewInventory = false;
    this.cloneInventory = this.cloneRecord(this.selectedInventory);

  }

  deleteInventory(inventory: Inventory) {
    this.selectedInventory = inventory;
    this.conf.confirm({
      message: 'Are you sure that you want to delete this data?',
      accept: () => {
        if (this.selectedInventory.productId == null)
          this.selectedInventory = new InventoryClass();
        else {
          this.globalService.deleteSomething("Inventories", this.selectedInventory.productId)
          let index = this.inventoryList.indexOf(this.selectedInventory);
          this.inventoryList = this.inventoryList.filter(
            (val, i) => i != index);
          this.inventory = null;
        }
        this.msgs = [];
        this.msgs.push({ severity: 'success', summary: 'Product deleted!' });
        this.selectedInventory = new InventoryClass();
      }
    });

  }

  cancelInventory() {
    if (this.isNewInventory == true) {
      this.display = false;
      this.userform.markAsPristine();
    }
    else {
      if (this.userform.dirty) {
        this.conf.confirm({
          message: 'Do you want to discard changes?',
          accept: () => {
            this.cancel()
          }
        });
      }
      else {
        this.display = false;
        this.edit = false;
      }
    }
  }

  cancel() {
    this.isNewInventory = false;
    let tmpInventoryList = [...this.inventoryList];
    tmpInventoryList[this.inventoryList.indexOf(this.selectedInventory)] = this.cloneInventory;
    this.inventoryList = tmpInventoryList;
    this.selectedInventory = this.cloneInventory;
    this.selectedInventory = new InventoryClass();
    this.edit = false;
    this.userform.markAsPristine();
  }

  onRowSelect() {
    this.isNewInventory = false;
    this.cloneInventory = this.cloneRecord(this.selectedInventory);
  }

  cloneRecord(r: Inventory): Inventory {
    let inventory = new InventoryClass();
    for (let prop in r) {
      inventory[prop] = r[prop];
    }
    return inventory;
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<Pagination<Inventory>>(this.entity, event.first, event.rows, this.searchQuery)
      .then(result => {
        this.inventoryList = result.result;
        this.totalRecord = result.totalCount;
      });
  }

  @ViewChild('dt') public dataTable: DataTable;
  search() {
    this.dataTable.reset();
  }

}
