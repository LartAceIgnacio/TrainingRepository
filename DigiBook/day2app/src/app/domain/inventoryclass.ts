import {Inventory} from '../domain/inventory'

export class InventoryClass implements Inventory
{
    constructor(public productId?, public productCode?,
    public productName?, public productDescription?, public qonHand?,
    public qonReserved?, public qonOrdered?, public dateCreated?,
    public dateModified?, public isActive?, public bin?,
    public ll?, public bb?, public rr? ) {
    }
}