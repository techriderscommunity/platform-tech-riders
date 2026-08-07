
import { bootstrapApplication, type BootstrapContext } from '@angular/platform-browser';
import { App } from './app/app';
import { config } from './app/app.config.server';


// Recibe el context y lo pasa a bootstrapApplication
const bootstrap = (context?: BootstrapContext) => bootstrapApplication(App, config, context);

export default bootstrap;
