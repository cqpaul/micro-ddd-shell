import React, { Component, Fragment } from 'react'
import './index.css'
// import { Statistic, Card, Icon } from 'antd'

export default class SummaryBox extends Component {
  render() {
    return (
      <Fragment>
        <span className="title-name">{this.props.title}</span>
        <br></br>
        <span className="number-name">{this.props.summary}</span>
        {/* <Card>
          <Statistic
            title={this.props.title}
            value={this.props.summary}
            precision={2}
            valueStyle={{ color: '#3f8600' }}
            prefix={<Icon type="arrow-up" />}
          />
        </Card> */}
      </Fragment>
    )
  }
}
