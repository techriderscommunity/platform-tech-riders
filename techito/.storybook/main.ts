import type { StorybookConfig } from '@storybook/angular';

const config: StorybookConfig = {
  stories: ['../src/app/shared/ui/**/*.stories.ts'],
  addons: ['@storybook/addon-a11y'],

  framework: {
    name: '@storybook/angular',
    options: {
      enableIvy: true,
    },
  },

  docs: {
    autodocs: true
  }
};

export default config;
