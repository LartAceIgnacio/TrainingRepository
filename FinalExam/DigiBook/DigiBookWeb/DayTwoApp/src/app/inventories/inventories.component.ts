import { Component, OnInit } from '@angular/core';
import { Inventory } from '../domain/inventories/inventory';
import { InventoryClass } from '../domain/inventories/inventoryclass';
import { MenuItem, ConfirmationService, DataTable, SelectItem } from 'primeng/primeng';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { GlobalService } from '../services/globalservice';
import { ViewChild } from '@angular/core';
import { PaginationResult } from '../domain/paginationresult';

@Component({
  selector: 'app-inventories',
  templateUrl: './inventories.component.html',
  styleUrls: ['./inventories.component.css'],
  providers: [GlobalService, ConfirmationService, FormBuilder]
})
export class InventoriesComponent implements OnInit {

  items: MenuItem[];
  home: MenuItem;
  inventoryList: Inventory[];
  selectedInventory: Inventory;
  isNewInventory: boolean;
  cloneInventory: Inventory;
  display: boolean;
  userform: FormGroup;
  userformEdit: FormGroup;
  isDelete: boolean;
  isEdit: boolean;
  edit: boolean;
  paginationResult: PaginationResult<Inventory>;
  // tslint:disable-next-line:no-inferrable-types
  searchFilter: string = '';
  // tslint:disable-next-line:no-inferrable-types
  totalRecords: number = 0;

  selectedFloor: SelectItem[];
  selectedBlock: SelectItem[];
  selectedRack: SelectItem[];

  Floor: string;
  Block: string;
  Rack: string;


  constructor(private globalService: GlobalService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder) {

    this.selectedFloor = [
      { label: '01', value: '01' },
      { label: '02', value: '02' },
      { label: '03', value: '03' },
      { label: '04', value: '04' },
      { label: '05', value: '05' },
    ];
    this.selectedBlock = [
      { label: 'B1', value: 'B1' },
      { label: 'B2', value: 'B2' },
      { label: 'B3', value: 'B3' },
      { label: 'B4', value: 'B4' },
      { label: 'B5', value: 'B5' },
      { label: 'B6', value: 'B6' },
      { label: 'B7', value: 'B7' },
      { label: 'B8', value: 'B8' },
      { label: 'B9', value: 'B9' }
    ];
    this.selectedRack = [
      { label: 'A', value: 'A' },
      { label: 'B', value: 'B' },
      { label: 'C', value: 'C' },
      { label: 'D', value: 'D' },
      { label: 'E', value: 'E' },
      { label: 'F', value: 'F' },
      { label: 'G', value: 'G' },
      { label: 'H', value: 'H' },
      { label: 'I', value: 'I' },
      { label: 'J', value: 'J' },
      { label: 'K', value: 'K' },
      { label: 'L', value: 'L' },
      { label: 'M', value: 'M' },
      { label: 'N', value: 'N' },
      { label: 'O', value: 'O' },
      { label: 'P', value: 'P' },
      { label: 'Q', value: 'Q' },
      { label: 'R', value: 'R' },
      { label: 'S', value: 'S' },
      { label: 'T', value: 'T' },
      { label: 'U', value: 'U' },
      { label: 'V', value: 'V' },
      { label: 'W', value: 'W' },
      { label: 'X', value: 'X' },
      { label: 'Y', value: 'Y' },
      { label: 'Z', value: 'Z' },
    ];

  }

  @ViewChild('dt') public dataTable: DataTable;

  ngOnInit() {

    this.items = [
      { label: 'Inventories' }
    ];
    this.home = { icon: 'fa fa-home', label: 'Home', routerLink: '/dashboard' };

    this.userform = this.fb.group({
      'productCode': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(8), Validators.minLength(8)])),
      'productName': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(60)])),
      'productDescription': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(250)])),
      'QOH': new FormControl(''),
      'QOR': new FormControl(''),
      'QOO': new FormControl(''),
      'Floor': new FormControl('', Validators.required),
      'Block': new FormControl('', Validators.required),
      'Rack': new FormControl('', Validators.required)
    });

    this.userformEdit = this.fb.group({
      'productCode': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(8), Validators.minLength(8)])),
      'productName': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(60)])),
      'productDescription': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(250)])),
      'QOH': new FormControl(''),
      'QOR': new FormControl(''),
      'QOO': new FormControl(''),
      'Floor': new FormControl('', Validators.required),
      'Block': new FormControl('', Validators.required),
      'Rack': new FormControl('', Validators.required)
    });
  }


  showDialog() {
    this.isEdit = false;
    this.isDelete = false;
    this.userform.enable();
    this.userform.markAsPristine();
    this.isNewInventory = true;
    this.Floor = null;
    this.Block = null;
    this.Rack = null;
    this.selectedInventory = new InventoryClass();
    this.display = true;
  }

  searchInventory() {
    if (this.searchFilter.length !== 1) {
      this.setCurrentPage(1);
    }
  }

  setCurrentPage(n: number) {
    this.dataTable.reset();
  }

  paginate(event) {
    this.globalService.getSomethingWithPagination<PaginationResult<Inventory>>('Inventories', event.first, event.rows,
      this.searchFilter.length === 1 ? '' : this.searchFilter).then(paginationResult => {
        this.paginationResult = paginationResult;
        this.inventoryList = this.paginationResult.results;
        this.totalRecords = this.paginationResult.totalRecords;
        for (let i = 0; i < this.inventoryList.length; i++) {
          this.inventoryList[i].dateCreated = this.inventoryList[i].dateCreated == null ? null :
            new Date(this.inventoryList[i].dateCreated).toLocaleDateString();
        }
      });
  }

  editInventory(inventoryToEdit: Inventory) {
    this.isEdit = true;
    this.isDelete = false;
    this.userformEdit.enable();
    this.isNewInventory = false;
    this.Floor = inventoryToEdit.bin.substring(0, 2);
    this.Block = inventoryToEdit.bin.substring(2, 4);
    this.Rack = inventoryToEdit.bin.substring(4);
    this.selectedInventory = this.cloneRecord(inventoryToEdit);
    this.cloneInventory = inventoryToEdit;
    this.edit = true;
  }

  deleteInventory(inventoryToDelete: Inventory) {
    this.isDelete = true;
    this.userform.disable();
    this.display = true;
    this.isNewInventory = false;
    this.Floor = inventoryToDelete.bin.substring(0, 2);
    this.Block = inventoryToDelete.bin.substring(2, 4);
    this.Rack = inventoryToDelete.bin.substring(4);
    this.selectedInventory = this.cloneRecord(inventoryToDelete);
    this.cloneInventory = inventoryToDelete;
    this.userform.markAsPristine();
  }

  confirmDelete() {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      accept: () => {
        this.globalService.deleteSomething<Inventory>('Inventories', this.selectedInventory.productId);
        const index = this.inventoryList.indexOf(this.cloneInventory);
        this.inventoryList = this.inventoryList.filter((val, i) => i !== index);
        this.selectedInventory = null;
        this.isNewInventory = false;
        this.display = false;

      }
    });
  }

  saveInventory() {
    const tmpInventoryList = [...this.inventoryList];
    this.selectedInventory.isActive = true;
    this.selectedInventory.bin = this.Floor + this.Block + this.Rack;
    if (this.isNewInventory) {
      this.globalService.postSomething<Inventory>('Inventories', this.selectedInventory)
        .then(inventory => {
          tmpInventoryList.push(inventory);
          this.inventoryList = tmpInventoryList;
          this.selectedInventory = null;
          this.display = false;
        });
    } else {
      this.globalService.putSomething<Inventory>('Inventories', this.selectedInventory.productId, this.selectedInventory)
        .then(inventory => {
          tmpInventoryList[this.inventoryList.indexOf(this.cloneInventory)] = this.selectedInventory;
          this.inventoryList = tmpInventoryList;
          this.selectedInventory = null;
          this.display = false;
        });
    }
    this.isNewInventory = false;
  }

  newSaveInventory() {
    this.userform.markAsPristine();
    const tmpInventoryList = [...this.inventoryList];
    this.selectedInventory.isActive = true;
    this.selectedInventory.bin = this.Floor + this.Block + this.Rack;
    this.selectedInventory.dateCreated = new Date();
    this.globalService.postSomething<Inventory>('Inventories', this.selectedInventory)
      .then(inventory => {
        tmpInventoryList.push(inventory);
        this.inventoryList = tmpInventoryList;
        this.selectedInventory = new InventoryClass;
        this.display = true;
      });
  }

  confirmCancel() {
    this.isNewInventory = false;
    const tmpInventoryList = [...this.inventoryList];
    tmpInventoryList[this.inventoryList.indexOf(this.selectedInventory)] = this.cloneInventory;
    this.inventoryList = tmpInventoryList;
    this.selectedInventory = Object.assign({}, this.cloneInventory);
    this.selectedInventory = new InventoryClass();
    this.display = false;
    this.edit = false;
    this.userformEdit.markAsPristine();
    this.userform.markAsPristine();
  }

  cancelInventory() {
    if (this.userform.dirty || this.userformEdit.dirty) {
      this.confirmationService.confirm({
        message: 'Are you sure that you want to discard changes?',
        accept: () => {
          this.confirmCancel();
        }
      });
    } else {
      this.display = false;
      this.edit = false;
    }
  }

  // onRowSelect() {
  //   this.isNewInventory = false;
  //   this.cloneInventory = this.cloneRecord(this.selectedInventory);
  // }

  cloneRecord(r: Inventory): Inventory {
    // tslint:disable-next-line:prefer-const
    let inventory = new InventoryClass();
    // tslint:disable-next-line:forin
    for (const prop in r) {
      inventory[prop] = r[prop];
    }
    return inventory;
  }
}



