import { Component, OnInit } from '@angular/core';
import { AuthService } from "../services/auth.service";

@Component({
  selector: 'app-departments',
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.css']
})
export class DepartmentsComponent implements OnInit {

  constructor(private auth: AuthService) { }

  ngOnInit() {
  }

}
