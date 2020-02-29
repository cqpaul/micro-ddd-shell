import React, { Component } from 'react'
import echarts from 'echarts'
import 'echarts-wordcloud'
import './index.css'

export default class WordCount extends Component {
  constructor(props) {
    super(props)

    this.state = {}
  }

  buildWordCountMap(textData) {
    var myChart = echarts.init(document.getElementById('wordChart'))
    let option = {
      tooltip: {
        show: true
      },
      series: [
        {
          type: 'wordCloud',
          gridSize: 6,
          shape: 'square',
          sizeRange: [10, 120],
          width: 800,
          height: 200,
          textStyle: {
            normal: {
              color: function() {
                return (
                  'rgb(' +
                  [
                    Math.round(Math.random() * 160),
                    Math.round(Math.random() * 160),
                    Math.round(Math.random() * 160)
                  ].join(',') +
                  ')'
                )
              }
            },
            emphasis: {
              shadowBlur: 10,
              shadowColor: '#333'
            }
          },
          data: textData
        }
      ]
    }
    myChart.setOption(option)
  }

  render() {
    return (
      <div>
        <span>宣传介绍词云</span>
        <div id="wordChart"></div>
      </div>
    )
  }
}
