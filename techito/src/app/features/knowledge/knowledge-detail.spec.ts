import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

import { KnowledgeDetail } from './knowledge-detail';

describe('KnowledgeDetail', () => {
  let fixture: ComponentFixture<KnowledgeDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KnowledgeDetail],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of({ get: () => 'intro-a-ia' }),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(KnowledgeDetail);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
