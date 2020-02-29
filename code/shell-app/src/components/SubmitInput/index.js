import React, { Component } from 'react'
import { Input } from 'antd'
import './index.css'
import api from '../../api'
import { notification } from 'antd'
const { Search } = Input

export default class SubmitInput extends Component {
  submitNewCrawlTask = value => {
    let newCrawlerTaskViewModel = {}
    newCrawlerTaskViewModel.VillageName = value
    api
      .callApi(
        'POST',
        '/CrawlerManagerService/CrawlerManager/StartNewCrawlerTask',
        newCrawlerTaskViewModel
      )
      .then(response => {
        console.log(response)
        notification.success({
          message: `New task for ${value} have been submitted.`,
          placement: 'bottomRight'
        })
      })
  }

  render() {
    return (
      <div className="submit-input">
        <Search
          placeholder="小区名称"
          onSearch={this.submitNewCrawlTask}
          enterButton="提交"
        />
      </div>
    )
  }
}
