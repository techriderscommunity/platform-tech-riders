import type { Preview } from '@storybook/angular';

const preview: Preview = {
  parameters: {
    layout: 'centered',
    docs: {
      description: {
        component: 'TechRiders Design System — WCAG AA accessible components using design tokens.',
      },
    },
    a11y: {
      config: {
        rules: [
          {
            id: 'color-contrast',
            enabled: true,
          },
          {
            id: 'button-name',
            enabled: true,
          },
          {
            id: 'link-name',
            enabled: true,
          },
        ],
      },
    },
  },
  decorators: [
    (story) => ({
      template: `<div class="storybook-container" style="padding: 2rem;">{{ story }}</div>`,
      props: { story },
    }),
  ],
};

export default preview;
