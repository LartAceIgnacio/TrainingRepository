import {Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-tally',
  templateUrl: './tally.component.html',
  styleUrls: ['./tally.component.css']
})
export class TallyComponent implements OnInit {
  @Input() icon : string;
  @Input() label : string;
  @Input() value: string;
  @Input() colour: string;

  constructor() { }

  ngOnInit() {
  }
}
