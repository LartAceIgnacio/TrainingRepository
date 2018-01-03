import { Component, OnInit, NgModule} from '@angular/core';
import {DropdownModule} from 'primeng/primeng';
import {SelectItem} from 'primeng/primeng';

interface City { 
  name: string,
  code: string
}


@Component({
  selector: 'app-inputs',
  templateUrl: './inputs.component.html',
  styleUrls: ['./inputs.component.css']
})

export class InputsComponent implements OnInit {
  
  selectedCity: City;
  cities: SelectItem[];

  constructor(){
    this.cities = [
      {label:'New York', value:{id:1, name: 'New York', code: 'NY'}},
      {label:'Rome', value:{id:2, name: 'Rome', code: 'RM'}},
      {label:'London', value:{id:3, name: 'London', code: 'LDN'}},
      {label:'Istanbul', value:{id:4, name: 'Istanbul', code: 'IST'}},
      {label:'Paris', value:{id:5, name: 'Paris', code: 'PRS'}}
  ];
  }

  ngOnInit() {
    
  }

}