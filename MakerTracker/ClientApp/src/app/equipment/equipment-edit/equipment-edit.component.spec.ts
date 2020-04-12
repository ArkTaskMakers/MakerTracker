import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { EquipmentEditComponent } from './equipment-edit.component';
import { EquipmentService } from 'src/app/services/backend/crud/equipment.service';

describe('EquipmentEditComponent', () => {
  let component: EquipmentEditComponent;
  let fixture: ComponentFixture<EquipmentEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EquipmentEditComponent],
      imports: [FormsModule, HttpClientTestingModule, RouterTestingModule],
      providers: [EquipmentService]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EquipmentEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
