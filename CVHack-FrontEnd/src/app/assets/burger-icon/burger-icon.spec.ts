import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BurgerIcon } from './burger-icon';

describe('BurgerIcon', () => {
  let component: BurgerIcon;
  let fixture: ComponentFixture<BurgerIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BurgerIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(BurgerIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
