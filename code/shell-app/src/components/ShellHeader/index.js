import React, { Component } from 'react'
import './index.css'

export default class Header extends Component {
  render() {
    return (
      <div>
        <h1 className="header-style">
          重庆房价分析仪表板
          <span className="small-text"> &nbsp; &nbsp; &nbsp;By Paul</span>
        </h1>
      </div>
    )
  }
}
