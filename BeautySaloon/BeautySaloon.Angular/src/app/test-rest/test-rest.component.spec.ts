import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestRestComponent } from './test-rest.component';

describe('TestRestComponent', () => {
  let component: TestRestComponent;
  let fixture: ComponentFixture<TestRestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TestRestComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestRestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
