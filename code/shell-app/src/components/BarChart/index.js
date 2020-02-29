import React, { Component } from 'react'
import echarts from 'echarts'
import './index.css'

export default class Bar extends Component {
  constructor(props) {
    super(props)

    this.state = {}
  }

  componentDidMount() {
    let barChart = echarts.init(document.getElementById('barChart'))
    barChart.setOption({
      xAxis: {
        type: 'category',
        data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
      },
      yAxis: {
        type: 'value'
      },
      series: [
        {
          data: [820, 932, 901, 934, 1290, 1330, 1320],
          type: 'line'
        }
      ]
    })
  }
  render() {
    return (
      <div>
        <div id="barChart">Bar Chart</div>
      </div>
    )
  }
}
