import React, { Component } from 'react'
import SummaryBox from '../SummaryBox'
import { Col } from 'antd'
import './index.css'
import api from '../../api'

export default class SubwayBox extends Component {
  state = {
    summaries: []
  }

  getHeaderSummaryData() {
    api
      .callApi('GET', '/ReportingService/Reporting/GetSummaryInfo')
      .then(response => {
        this.setState({ summaries: response })
      })
  }

  render() {
    const items = this.state.summaries.map(d => (
      <Col key={d.title} span={3}>
        <SummaryBox title={d.title} summary={d.count}></SummaryBox>
      </Col>
    ))

    return (
      <div className="subway-box">
        <Col span={3}></Col>
        {items}
      </div>
    )
  }
}
