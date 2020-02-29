import React, { Component } from 'react'
import { Row, Col, Spin } from 'antd'
import { Button, notification } from 'antd'
import SearchBox from '../SearchBox'
import SubmitInput from '../SubmitInput'
import SubwayBox from '../SubwayBox'
import SummaryList from '../SummaryList'
import WordCount from '../WordCount'
import MapChart from '../MapChart'
import api from '../../api'
import './index.css'

const signalR = require('@microsoft/signalr')
export default class ContentBody extends Component {
  constructor(props) {
    super(props)

    this.state = {
      allVillageNameList: [],
      selectedVillageName: undefined,
      summaryListLoading: true,
      mapChartLoading: true,
      wordCountLoading: true,
      filteredPosition: undefined,
      filteredPositionIsShow: 'none',
      textData: [],
      mapMarkerData: { lng: 106.45, lat: 29.53 }
    }
    // Bind handler
    this.handleVillageSelect = this.handleVillageSelect.bind(this)
    this.finishedFetching = this.finishedFetching.bind(this)
    this.mapChartFinishedLoading = this.mapChartFinishedLoading.bind(this)
    this.getFilteredPosition = this.getFilteredPosition.bind(this)
    this.removeFilteredPosition = this.removeFilteredPosition.bind(this)
    this.handleVillageSearch = this.handleVillageSearch.bind(this)
  }

  componentDidMount() {
    this.refreshAll()
    let signalrUrl = `${api.singalRUrl}/messageHub`
    // For SingalR
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(signalrUrl)
      .configureLogging(signalR.LogLevel.Information)
      .build()
    connection.start().then(function() {
      console.log('connected')
    })

    connection.on('FinishedEtl', response => {
      notification.info({
        message: `New data is coming, will refresh in 5 minute.`,
        placement: 'bottomRight'
      })
      setTimeout(() => {
        this.refreshAll()
      }, 5000)
    })

    connection.on('ReceiveMessage', (user, message) => {
      console.log(user)
      console.log(message)
    })
  }

  refreshAll() {
    this.getAllVillageNameList()
    this.refs.summaryList.getSummaryListData(this.state.selectedVillageName)
    this.refs.subwayBox.getHeaderSummaryData()
    this.getVillageTitleTextData()
    this.refs.mapChart.buildMap(undefined)
  }

  handleVillageSelect(villageName) {
    this.setState({ summaryListLoading: true })
    this.setState({ selectedVillageName: villageName })
    this.refs.summaryList.getSummaryListData(villageName)
    console.log(villageName)
    // refresh word count & baidu map
    this.setState({ wordCountLoading: true })
    this.setState({ mapChartLoading: true })
    this.getVillageTitleTextData(villageName)
    this.refs.mapChart.buildMap(villageName)
    // this.setState({ summaryListLoading: false })
  }

  handleVillageSearch(value) {
    console.log(value.length)
    if (value.length >= 1) {
      api
        .callApi(
          'GET',
          `/ReportingService/Reporting/GetCurrentAllVillages?queryString=${value}`
        )
        .then(response => {
          this.setState({ allVillageNameList: response })
        })
    }
  }

  getFilteredPosition(filteredPosition) {
    this.setState({ filteredPosition: filteredPosition })
    this.setState({ filteredPositionIsShow: 'block' })
    this.setState({ wordCountLoading: true })
    this.getVillageTitleTextData(filteredPosition)
    this.refs.mapChart.buildMap(filteredPosition)
  }

  getAllVillageNameList() {
    api
      .callApi('GET', '/ReportingService/Reporting/GetCurrentAllVillages')
      .then(response => {
        this.setState({ allVillageNameList: response })
      })
  }

  getVillageTitleTextData(villageName) {
    let getUrl = '/ReportingService/Reporting/GetWordCountDictionary'
    if (villageName !== 'undefined' && villageName !== undefined) {
      getUrl = `/ReportingService/Reporting/GetWordCountDictionary?villageName=${villageName}`
    }
    api
      .callApi('GET', getUrl)
      .then(response => {
        let result = this.convertKey(response, ['name', 'value'])
        this.setState({ textData: result })
      })
      .then(() => {
        this.refs.wordCount.buildWordCountMap(this.state.textData)
        this.setState({ wordCountLoading: false })
      })
  }

  convertKey(arr, key) {
    let newArr = []
    arr.forEach((item, index) => {
      let newObj = {}
      for (var i = 0; i < key.length; i++) {
        newObj[key[i]] = item[Object.keys(item)[i]]
      }
      newArr.push(newObj)
    })
    return newArr
  }

  finishedFetching() {
    this.setState({ summaryListLoading: false })
  }
  mapChartFinishedLoading() {
    this.setState({ mapChartLoading: false })
  }

  removeFilteredPosition() {
    this.setState({ filteredPosition: undefined })
    this.setState({ filteredPositionIsShow: 'none' })
    this.getVillageTitleTextData(undefined)
    this.refs.mapChart.buildMap(undefined)
  }

  render() {
    return (
      <div className="content-body">
        <Row>
          <Col span={4}>
            <div className="search-box">
              <SearchBox
                handleVillageSelect={this.handleVillageSelect}
                handleVillageSearch={this.handleVillageSearch}
                villageNameList={this.state.allVillageNameList}
                selectedVillageName={this.state.selectedVillageName}
              ></SearchBox>
            </div>
          </Col>
          <Col span={16}>
            <div>
              <SubwayBox ref="subwayBox"></SubwayBox>
            </div>
          </Col>
          <Col span={4}>
            <div>
              <SubmitInput></SubmitInput>
            </div>
          </Col>
        </Row>
        <Row>
          <Col span={11}>
            <Spin spinning={this.state.summaryListLoading}>
              <SummaryList
                ref="summaryList"
                finishedFetching={this.finishedFetching}
                selectedVillageName={this.state.selectedVillageName}
                getFilteredPosition={this.getFilteredPosition}
              ></SummaryList>
            </Spin>
          </Col>
          <Col span={13}>
            <div className="right-panel">
              <Row style={{ height: '30px' }}>
                <Button
                  style={{
                    display: this.state.filteredPositionIsShow,
                    float: 'right',
                    marginRight: '27px'
                  }}
                  onClick={this.removeFilteredPosition}
                  type="primary"
                  icon="smile"
                  size="small"
                >
                  {this.state.filteredPosition}
                </Button>
              </Row>
              <Row style={{ height: '200px' }}>
                <WordCount
                  spinning={this.state.wordCountLoading}
                  ref="wordCount"
                  textData={this.state.textData}
                ></WordCount>
              </Row>
              <Row style={{ height: '500px', paddingTop: '20px' }}>
                <MapChart
                  spinning={this.state.mapChartLoading}
                  ref="mapChart"
                  finishedFetching={this.mapChartFinishedLoading}
                  mapMarkerData={this.mapMarkerData}
                ></MapChart>
              </Row>
            </div>
          </Col>
        </Row>
      </div>
    )
  }
}
