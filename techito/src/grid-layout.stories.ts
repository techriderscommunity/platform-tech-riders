import { Meta, StoryObj } from '@storybook/angular';
import { Component } from '@angular/core';

@Component({
  selector: 'app-grid-layout-demo',
  standalone: true,
  template: `
    <div [class]="gridClass">
      @for (item of items; track item.title) {
        <div class="card">
          <h4>{{ item.title }}</h4>
          <p>{{ item.content }}</p>
        </div>
      }
    </div>
  `,
  styles: [`
    :host { display: block; }
  `]
})
class GridLayoutDemoComponent {
  gridClass = 'grid grid-auto-fit gap-4';
  items = [
    { title: 'Item 1', content: 'This is the first grid item.' },
    { title: 'Item 2', content: 'This is the second grid item.' },
    { title: 'Item 3', content: 'This is the third grid item.' }
  ];
}

const meta: Meta<GridLayoutDemoComponent> = {
  title: 'Components/Grid Layout',
  component: GridLayoutDemoComponent,
  tags: ['autodocs']
};

export default meta;
type Story = StoryObj<GridLayoutDemoComponent>;

export const AutoFit: Story = {
  render: () => ({
    template: `
      <div class="grid grid-auto-fit gap-4">
        <div class="card">
          <h4>Item 1</h4>
          <p>Responsive grid that auto-fits columns based on space.</p>
        </div>
        <div class="card">
          <h4>Item 2</h4>
          <p>Items wrap to next row on smaller screens.</p>
        </div>
        <div class="card">
          <h4>Item 3</h4>
          <p>Perfect for responsive layouts.</p>
        </div>
        <div class="card">
          <h4>Item 4</h4>
          <p>Add more items as needed.</p>
        </div>
      </div>
    `,
    styles: [`
      :host { display: block; }
    `]
  })
};

export const TwoColumns: Story = {
  render: () => ({
    template: `
      <div class="grid grid-cols-2 gap-4">
        <div class="card">
          <h4>Left Column</h4>
          <p>This is the left column of a 2-column grid.</p>
        </div>
        <div class="card">
          <h4>Right Column</h4>
          <p>This is the right column. Collapses to 1 column on mobile.</p>
        </div>
      </div>
    `,
    styles: [`
      :host { display: block; }
    `]
  })
};

export const ThreeColumns: Story = {
  render: () => ({
    template: `
      <div class="grid grid-cols-3 gap-4">
        <div class="card">
          <h4>Column 1</h4>
          <p>First column</p>
        </div>
        <div class="card">
          <h4>Column 2</h4>
          <p>Second column</p>
        </div>
        <div class="card">
          <h4>Column 3</h4>
          <p>Third column</p>
        </div>
      </div>
    `,
    styles: [`
      :host { display: block; }
    `]
  })
};

export const WithSpacing: Story = {
  render: () => ({
    template: `
      <div class="grid grid-auto-fit gap-6">
        <div class="card">
          <h4>Larger Gap</h4>
          <p>This grid uses gap-6 (24px) spacing between items.</p>
        </div>
        <div class="card">
          <h4>Larger Gap</h4>
          <p>More breathing room between cards.</p>
        </div>
        <div class="card">
          <h4>Larger Gap</h4>
          <p>Better for visual hierarchy.</p>
        </div>
      </div>
    `,
    styles: [`
      :host { display: block; }
    `]
  })
};

export const Mixed: Story = {
  render: () => ({
    template: `
      <div class="container">
        <h2>Grid Layout Examples</h2>

        <h3 style="margin-top: var(--space-6); margin-bottom: var(--space-3);">Auto-Fit (Responsive)</h3>
        <div class="grid grid-auto-fit gap-4">
          <div class="card"><h4>Item 1</h4></div>
          <div class="card"><h4>Item 2</h4></div>
          <div class="card"><h4>Item 3</h4></div>
        </div>

        <h3 style="margin-top: var(--space-6); margin-bottom: var(--space-3);">2 Columns</h3>
        <div class="grid grid-cols-2 gap-4">
          <div class="card"><h4>Left</h4></div>
          <div class="card"><h4>Right</h4></div>
        </div>

        <h3 style="margin-top: var(--space-6); margin-bottom: var(--space-3);">3 Columns</h3>
        <div class="grid grid-cols-3 gap-4">
          <div class="card"><h4>Col 1</h4></div>
          <div class="card"><h4>Col 2</h4></div>
          <div class="card"><h4>Col 3</h4></div>
        </div>
      </div>
    `,
    styles: [`
      :host { display: block; }
    `]
  })
};
