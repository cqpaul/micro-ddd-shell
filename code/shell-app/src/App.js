import React from 'react'
import './App.css'

import { ShellHeader, ContentBody } from './components'

function App() {
  return (
    <div>
      <div className="layout">
        <div className="shell-header">
          <ShellHeader></ShellHeader>
        </div>
        <div className="content-body">
          <ContentBody></ContentBody>
        </div>
      </div>
    </div>
  )
}

export default App
