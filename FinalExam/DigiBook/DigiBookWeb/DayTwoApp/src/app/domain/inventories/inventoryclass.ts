import { Inventory } from '../inventories/inventory';
export class InventoryClass implements Inventory {

    constructor(public productId?, public productCode?, public productName?, public productDescription?, public QOH?,
        public QOR?, public QOO?, public isActive?, public bin?) {}
}
