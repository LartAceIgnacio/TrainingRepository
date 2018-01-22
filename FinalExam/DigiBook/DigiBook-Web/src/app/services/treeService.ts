import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { TreeNode } from "primeng/primeng";



@Injectable()
export class NodeService {
    
    constructor(private http: Http) {}

    getFilesystem() {
        return this.http.get('assets/data/trees.json')
                    .toPromise()
                    .then(res => <TreeNode[]> res.json().data);
    }
}