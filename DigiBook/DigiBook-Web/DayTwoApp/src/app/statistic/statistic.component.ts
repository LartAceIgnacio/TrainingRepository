import { Component, OnInit, Input} from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'at-statistic',
  templateUrl: './statistic.component.html',
  styleUrls: ['./statistic.component.css']
})
export class StatisticComponent implements OnInit {

  @Input() icon: string;
  @Input() label: string;
  @Input() value: string;
  @Input() colour: string;

  constructor() { }

  ngOnInit() {
  }

}
