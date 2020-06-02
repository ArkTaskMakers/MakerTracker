import { Component, OnDestroy } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ButtonColumnParams } from './button-column-params';

@Component({
  selector: 'app-button-column',
  templateUrl: './button-column.component.html',
  styleUrls: ['./button-column.component.css']
})
export class ButtonColumnComponent implements ICellRendererAngularComp, OnDestroy {
  public params: ButtonColumnParams;

  agInit(params: any): void {
    this.params = params;
  }

  ngOnDestroy() {
    // no need to remove the button click handler
    // https://stackoverflow.com/questions/49083993/does-angular-automatically-remove-template-event-listeners
  }

  refresh(params: ButtonColumnParams): boolean {
    this.params = params;
    return true;
  }

  afterGuiAttached?(params?: import('ag-grid-community').IAfterGuiAttachedParams): void {}
}
