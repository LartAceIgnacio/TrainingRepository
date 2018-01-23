import { Inventory } from '../inventories/inventory';
export class InventoryClass implements Inventory {

    constructor(public productId?, public productCode?, public productName?, public productDescription?, public qoh?,
        public qor?, public qoo?, public dateCreated?, public dateModified?, public isActive?, public bin?) {}
}
