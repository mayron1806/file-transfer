import { createRoot } from 'react-dom/client';
import * as React from 'react';
import { ChakraProvider } from '@chakra-ui/react';

const root = createRoot(document.body);
root.render(
  <React.StrictMode>
    <ChakraProvider>
      <h1>💖 Hello World!</h1>
    </ChakraProvider>
  </React.StrictMode>
);