import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  data: any;

  hoursByProject = [
    {id:1, name: 'Payroll App', hoursSpent:8},
    {id:2, name: 'Agile Times App', hoursSpent: 16},
    {id:3, name: 'Point of Sale App', hoursSpent: 24}
  ];

  hoursByProjectChartData = {
    labels:['Payroll App', 'Agile Times App', 'Point ofSales App'],
    datasets: [
      {
        data: [8,16,24],
        backgroundColor:["#3366cc","#cc3912","#ff9900"]
      }
    ]
  }; 
  constructor() {
   
  }

  ngOnInit() {

  }
}
